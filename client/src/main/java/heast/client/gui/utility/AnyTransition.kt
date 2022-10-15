package heast.client.gui.utility

import javafx.animation.Transition
import javafx.util.Duration

class AnyTransition(private val callback: (t: Double) -> Unit) : Transition() {
	var from : Double = 0.0
	var to : Double = 1.0
	var duration : Duration = Duration.seconds(1.0)

	override fun interpolate(frac : Double) {
		callback(
			from + (to - from) * frac
		)
	}

	override fun play() {
		this.cycleDuration = duration
		super.play()
	}
}