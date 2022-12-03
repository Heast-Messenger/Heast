package heast.client.gui.utility

import kotlin.math.pow

object Interpolator {
	val elastic = object : javafx.animation.Interpolator() {
		override fun curve(t : Double) : Double {
			val c1 = 1.70158
			val c2 = c1 * 1.525

			return if (t < 0.5) {
				(2*t).pow(2) * ((c2 + 1) * 2 * t - c2) / 2
			} else {
				( 2* t - 2).pow(2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2
			} / 2
		}
	}

	val cubic = object : javafx.animation.Interpolator() {
		override fun curve(t : Double) : Double {
			return if (t < 0.5) {
				4 * t.pow(3)
			} else {
				1 - (-2 * t + 2).pow(3) / 2
			}
		}
	}
}