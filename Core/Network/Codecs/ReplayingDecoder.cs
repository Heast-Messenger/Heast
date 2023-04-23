using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace Core.Network.codecs;

public abstract class ReplayingDecoder<TState> : ByteToMessageDecoder
{
    private int _checkpoint;
    private bool _replayRequested;

    protected ReplayingDecoder() : this(default) { }

    protected ReplayingDecoder(TState? initialState)
    {
        State = initialState;
    }

    protected TState? State { get; private set; }

    protected void Checkpoint()
    {
        _checkpoint = InternalBuffer.ReaderIndex;
    }

    protected void Checkpoint(TState newState)
    {
        Checkpoint();
        State = newState;
    }

    protected void RequestReplay()
    {
        _replayRequested = true;
    }

    protected override void CallDecode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        try
        {
            while (input.IsReadable())
            {
                _replayRequested = false;
                var oldReaderIndex = _checkpoint = input.ReaderIndex;
                var outSize = output.Count;
                var oldState = State;
                var oldInputLength = input.ReadableBytes;
                Decode(context, input, output);

                if (_replayRequested)
                {
                    // Check if this handler was removed before continuing the loop.
                    // If it was removed, it is not safe to continue to operate on the buffer.
                    //
                    // See https://github.com/netty/netty/issues/1664
                    if (context.Removed) break;

                    // Return to the checkpoint (or oldPosition) and retry.
                    var restorationPoint = _checkpoint;
                    if (restorationPoint >= 0) input.SetReaderIndex(restorationPoint);

                    // Called by cleanup() - no need to maintain the readerIndex
                    // anymore because the buffer has been released already.
                    break;
                }

                // Check if this handler was removed before continuing the loop.
                // If it was removed, it is not safe to continue to operate on the buffer.
                //
                // See https://github.com/netty/netty/issues/1664
                if (context.Removed) break;

                if (outSize == output.Count)
                {
                    if (oldInputLength == input.ReadableBytes &&
                        EqualityComparer<TState>.Default.Equals(oldState, State))
                        throw new DecoderException(
                            $"{GetType().Name}.Decode() must consume the inbound data or change its state if it did not decode anything.");
                    // Previous data has been discarded or caused state transition.
                    // Probably it is reading on.
                    continue;
                }

                if (oldReaderIndex == input.ReaderIndex &&
                    EqualityComparer<TState>.Default.Equals(oldState, State))
                    throw new DecoderException(
                        $"{GetType().Name}.Decode() method must consume the inbound data or change its state if it decoded something.");

                if (SingleDecode) break;
            }
        }
        catch (DecoderException)
        {
            throw;
        }
        catch (Exception cause)
        {
            throw new DecoderException(cause);
        }
    }
}