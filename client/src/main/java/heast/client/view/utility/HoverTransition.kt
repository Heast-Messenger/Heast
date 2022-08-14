package heast.client.view.utility

import javafx.animation.Interpolator
import javafx.animation.TranslateTransition
import javafx.scene.Node
import javafx.util.Duration

object HoverTransition {
	fun onMouseEntered(node: Node) {
		TranslateTransition().apply {
			this.node = node
			this.fromY = node.translateY
			this.toY = -3.0
			this.duration = Duration.seconds(0.1)
			this.interpolator = Interpolator.EASE_OUT
		}.play()
	}

	fun onMouseExited(node: Node) {
		TranslateTransition().apply {
			this.node = node
			this.fromY = node.translateY
			this.toY = 0.0
			this.duration = Duration.seconds(0.1)
			this.interpolator = Interpolator.EASE_OUT
		}.play()
	}
}