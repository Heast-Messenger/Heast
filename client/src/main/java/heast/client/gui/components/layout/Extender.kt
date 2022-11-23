package heast.client.gui.components.layout

import javafx.scene.layout.HBox
import javafx.scene.layout.Priority
import javafx.scene.layout.Region

object Extender {
	fun hbox() = Region().apply {
		HBox.setHgrow(this, Priority.ALWAYS)
	}

	fun vbox() = Region().apply {
		HBox.setHgrow(this, Priority.ALWAYS)
	}
}