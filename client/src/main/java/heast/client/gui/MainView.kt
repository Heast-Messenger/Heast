package heast.client.gui

import javafx.beans.binding.Bindings
import javafx.geometry.Insets
import javafx.scene.Node
import javafx.scene.layout.*
import heast.client.model.Current
import heast.client.model.Settings
import heast.client.gui.utility.MultiStack

object MainView : BorderPane() {
	val stackPane: StackPane

	fun reset(){
		setView(HomeView)
		SidebarView.hide()
		NavigationView.toggleGroup.reset()
		Current.panel.set("Welcome!")
	}

	init {
		this.center = BorderPane().apply {
			this.top = MenubarView
			this.center = StackPane(
				// <current view>
			).apply {
				stackPane = this
				this.padding = Insets(0.0, 20.0, 20.0, 0.0)
			}
		}
		this.left = NavigationView
		this.right = SidebarView

		setView(HomeView)
		SidebarView.hide()
		Current.panel.set("Welcome!")

		this.backgroundProperty().bind(
			Bindings.createObjectBinding({
				Background(
					BackgroundFill(
						Settings.colors["Primary Color"]!!.color.value,
						CornerRadii.EMPTY,
						Insets.EMPTY
					)
				)
			}, Settings.colors["Primary Color"]!!.color)
		)
	}

	fun setView(view: Node, scale: Boolean = true, fade: Boolean = true) {
		MultiStack.setStackPaneView(view, stackPane, scale, fade)
	}
}