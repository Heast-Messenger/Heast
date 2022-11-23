package heast.client.gui.components.layout

import javafx.scene.layout.Region

object Pad {
	fun hbox(size: Double) = Region().apply {
		this.prefWidth = size
	}

	fun vbox(size: Double) = Region().apply {
		this.prefHeight = size
	}
}