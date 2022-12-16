package heast.client.gui.registry

import javafx.animation.Interpolator
import kotlin.math.pow
import kotlin.math.sin
import kotlin.random.Random
import javafx.animation.Interpolator as JFXInterpolator

object Interpolators {
	val ELASTIC = object : JFXInterpolator() {
		val c1 = 1.70158
		val c2 = c1 * 1.525
		override fun curve(t : Double) =
			if (t < 0.5) {
				(2*t).pow(2) * ((c2 + 1) * 2 * t - c2) / 2
			} else {
				( 2* t - 2).pow(2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2
			} / 2
	}

	val CUBIC = object : JFXInterpolator() {
		override fun curve(t : Double) =
			if (t < 0.5) {
				4 * t.pow(3)
			} else {
				1 - (-2 * t + 2).pow(3) / 2
			}
	}

	val NAVIGATOR = object : Interpolator() {
		val rd = Random.nextInt(0, 2) * 2 - 1 // random direction
		override fun curve(t : Double) =
			rd * sin(10*t) *2.0.pow(-5*t)
	}
}