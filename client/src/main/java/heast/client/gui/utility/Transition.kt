package heast.client.gui.utility

import javafx.animation.Transition
import javafx.util.Duration

class Transition(private val cb: (t: Double) -> Unit) : Transition() {
	var from = 0.0
	var to = 1.0
	var duration = Duration.seconds(1.0)

	override fun interpolate(frac : Double) {
		cb( from + (to - from) * frac )
	}

	override fun play() {
		this.cycleDuration = duration
		super.play()
	}
}