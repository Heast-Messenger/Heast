using DotNetty.Codecs;

namespace Core.network.codecs;

using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;

public abstract class ReplayingDecoder<TState> : ByteToMessageDecoder {
    protected TState? State { get; private set; }
    private int _checkpoint;
    private bool _replayRequested;

    protected ReplayingDecoder() : this(default) {
    }

    protected ReplayingDecoder(TState? initialState) {
        this.State = initialState;
    }

    protected void Checkpoint() {
        this._checkpoint = this.InternalBuffer.ReaderIndex;
    }

    protected void Checkpoint(TState newState) {
        this.Checkpoint();
        this.State = newState;
    }

    protected void RequestReplay() {
        this._replayRequested = true;
    }

    protected override void CallDecode(IChannelHandlerContext context, IByteBuffer input, List<object> output) {
        try {
            while (input.IsReadable()) {
                this._replayRequested = false;
                var oldReaderIndex = this._checkpoint = input.ReaderIndex;
                var outSize = output.Count;
                var oldState = this.State;
                var oldInputLength = input.ReadableBytes;
                this.Decode(context, input, output);

                if (this._replayRequested) {
                    // Check if this handler was removed before continuing the loop.
                    // If it was removed, it is not safe to continue to operate on the buffer.
                    //
                    // See https://github.com/netty/netty/issues/1664
                    if (context.Removed) {
                        break;
                    }

                    // Return to the checkpoint (or oldPosition) and retry.
                    var restorationPoint = this._checkpoint;
                    if (restorationPoint >= 0) {
                        input.SetReaderIndex(restorationPoint);
                    }
                    else {
                        // Called by cleanup() - no need to maintain the readerIndex
                        // anymore because the buffer has been released already.
                    }

                    break;
                }

                // Check if this handler was removed before continuing the loop.
                // If it was removed, it is not safe to continue to operate on the buffer.
                //
                // See https://github.com/netty/netty/issues/1664
                if (context.Removed) {
                    break;
                }

                if (outSize == output.Count) {
                    if (oldInputLength == input.ReadableBytes &&
                        EqualityComparer<TState>.Default.Equals(oldState, this.State)) {
                        throw new DecoderException(
                            $"{this.GetType().Name}.Decode() must consume the inbound data or change its state if it did not decode anything.");
                    }
                    else {
                        // Previous data has been discarded or caused state transition.
                        // Probably it is reading on.
                        continue;
                    }
                }

                if (oldReaderIndex == input.ReaderIndex &&
                    EqualityComparer<TState>.Default.Equals(oldState, this.State)) {
                    throw new DecoderException(
                        $"{this.GetType().Name}.Decode() method must consume the inbound data or change its state if it decoded something.");
                }

                if (this.SingleDecode) {
                    break;
                }
            }
        }
        catch (DecoderException) {
            throw;
        }
        catch (Exception cause) {
            throw new DecoderException(cause);
        }
    }
}