package heast.client.view.utility

import javafx.animation.FadeTransition
import javafx.animation.PauseTransition
import javafx.animation.ScaleTransition
import javafx.event.EventHandler
import javafx.scene.Node
import javafx.scene.layout.StackPane
import javafx.util.Duration

object MultiStack {
	fun setStackPaneView(node: Node, pane: StackPane, scale: Boolean = true, fade : Boolean = true) {
		addInternal(node, pane, scale, fade) {
			pane.children.removeIf { node ->
				node != pane.children.last()
			}
		}
	}

	fun addStackPaneView(node: Node, pane: StackPane, scale: Boolean = true, fade: Boolean = true) {
		addInternal(node, pane, scale, fade)
	}

	fun removeStackPaneView(node: Node, pane: StackPane, scale: Boolean = true, fade: Boolean = true) {
		removeInternal(node, pane, scale, fade)
	}

	private fun addInternal(view: Node, pane: StackPane, scale: Boolean = true, fade: Boolean = true, onFinished: () -> Unit = {}) {
		pane.children.remove(view)
		pane.children.add(
			view.apply {
				if (scale) {
					ScaleTransition().apply {
						this.node = view
						this.duration = Duration.seconds(0.5)
						this.fromX = 0.8
						this.fromY = 0.8
						this.toX = 1.0
						this.toY = 1.0
						this.interpolator = Interpolator.easeInOutBack
					}.play()
				}
				if (fade) {
					FadeTransition().apply {
						this.node = view
						this.duration = Duration.seconds(0.5)
						this.fromValue = 0.0
						this.toValue = 1.0
						this.interpolator = Interpolator.easeOut
					}.play()
				}
				PauseTransition().apply {
					this.duration = Duration.seconds(0.5)
					this.onFinished = EventHandler {
						onFinished()
					}
				}.play()
			}
		)
	}

	private fun removeInternal(view: Node, pane: StackPane, scale: Boolean = true, fade: Boolean, onFinished: () -> Unit = {}) {
		if (scale) {
			ScaleTransition().apply {
				this.node = view
				this.duration = Duration.seconds(0.5)
				this.fromX = 1.0
				this.fromY = 1.0
				this.toX = 0.0
				this.toY = 0.0
				this.interpolator = Interpolator.easeInBack
			}.play()
		}
		if (fade) {
			FadeTransition().apply {
				this.node = view
				this.duration = Duration.seconds(0.5)
				this.fromValue = 1.0
				this.toValue = 0.0
			}.play()
		}
		PauseTransition().apply {
			this.duration = Duration.seconds(0.5)
			this.onFinished = EventHandler {
				pane.children.remove(
					view
				)
				onFinished()
			}
		}.play()
	}
}