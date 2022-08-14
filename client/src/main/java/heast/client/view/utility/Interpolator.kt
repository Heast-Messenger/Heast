package heast.client.view.utility

import javafx.animation.Interpolator
import java.lang.Math.pow
import kotlin.math.pow

object Interpolator {
	val easeInOutBack = object : Interpolator() {
		override fun curve(t : Double) : Double {
			val c1 = 1.70158
			val c2 = c1 * 1.525

			return if (t < 0.5) {
				(2 * t).pow(2) * ((c2 + 1) * 2 * t - c2) / 2
			} else {
				(2 * t - 2).pow(2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2
			} / 2
		}
	}

	val easeInBack = object : Interpolator() {
		override fun curve(t : Double) : Double {
			val c1 = 1.70158
			val c3 = c1 + 1.0
			return c3 * t * t * t - c1 * t * t
		}
	}

	val easeOut = object : Interpolator() {
		override fun curve(t: Double) : Double {
			return if(t == 1.0) 1.0 else 1.0 - 2.0.pow(-10 * t)
        }
	}
}