package heast.client.gui.components.layout

import javafx.scene.layout.HBox
import javafx.scene.layout.Priority
import javafx.scene.layout.Region
import javafx.scene.layout.VBox

object Extender {
	fun hbox() = Region().apply {
		HBox.setHgrow(this, Priority.ALWAYS)
	}

	fun vbox() = Region().apply {
		VBox.setVgrow(this, Priority.ALWAYS)
	}
}