package heast.client.gui.scenes

import heast.client.gui.components.layout.Pad
import heast.client.gui.components.welcome.*
import heast.client.gui.registry.Icons
import heast.client.gui.registry.Icons.toImg
import javafx.geometry.Pos
import javafx.scene.image.ImageView
import javafx.scene.layout.*

object Welcome : BorderPane() {
	init {
		this.top = VBox().apply {
			this.children.addAll(
				Pad.vbox(40.0),
				Title()
			)

			this.spacing = 10.0
			this.alignment = Pos.TOP_CENTER
		}

		this.center = ImageView(Icons.logo["big"]!!.toImg()).apply {
			this.fitWidth = 280.0
			this.fitHeight = 280.0
		}

		this.bottom = HBox().apply {

		}
	}
}