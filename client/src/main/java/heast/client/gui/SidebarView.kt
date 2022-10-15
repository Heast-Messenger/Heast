package heast.client.gui

import javafx.animation.FadeTransition
import javafx.beans.binding.Bindings
import javafx.geometry.Insets
import javafx.scene.Node
import javafx.scene.layout.Background
import javafx.scene.layout.BackgroundFill
import javafx.scene.layout.CornerRadii
import javafx.scene.layout.StackPane
import javafx.util.Duration
import heast.client.model.Settings
import heast.client.gui.utility.AnyTransition
import heast.client.gui.utility.Interpolator
import heast.client.gui.utility.MultiStack

object SidebarView : StackPane() {
	private var panelWidth: Double = 300.0

	init {
		this.minWidth = 0.0
		this.padding = Insets(20.0)
		this.prefWidth = panelWidth
		this.backgroundProperty().bind(
			Bindings.createObjectBinding({
				Background(
					BackgroundFill(
						Settings.colors["Secondary Color"]!!.color.value,
						CornerRadii.EMPTY,
						Insets.EMPTY
					)
				)
			}, Settings.colors["Secondary Color"]!!.color)
		)
	}

	fun hide() {
		FadeTransition().apply {
			this.node = this@SidebarView.getContent()
			this.duration = Duration.seconds(0.3)
			this.fromValue = this@SidebarView.getContent()?.opacity ?: 1.0
			this.toValue = 0.0
		}.play()

		AnyTransition {
			this.maxWidth = it
		}.apply {
			this.duration = Duration.seconds(0.5)
			this.from = this@SidebarView.maxWidth
			this.to = 0.0
			this.interpolator = Interpolator.easeInOutBack
		}.play()
	}

	fun show(view: Node, size: Double = panelWidth) {
		this.setView(view)
		this.getContent()?.opacity = 1.0

		AnyTransition {
			this.maxWidth = it
		}.apply {
			this.duration = Duration.seconds(0.5)
			this.from = this@SidebarView.maxWidth
			this.to = size
			this.interpolator = Interpolator.easeInOutBack
		}.play()
	}

	private fun getContent(): Node? {
		return this.children.firstOrNull()
	}

	private fun setView(view: Node) {
		MultiStack.setStackPaneView(view, this)
	}
}

