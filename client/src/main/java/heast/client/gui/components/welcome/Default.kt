package heast.client.gui.components.welcome

import heast.client.gui.registry.Icons
import javafx.scene.Parent
import javafx.scene.layout.BorderPane
import javafx.scene.layout.HBox
import kotlin.reflect.KClass

abstract class Default : BorderPane() {

	abstract val back: KClass<out Parent>?

	init {
		this.top = HBox(
			Navigator(Icons.menu["back"]!!, back)
		).apply {
			this.styleClass.addAll(
				"spacing-3",
				"align-center-right",
				"px-4")
		}
	}
}