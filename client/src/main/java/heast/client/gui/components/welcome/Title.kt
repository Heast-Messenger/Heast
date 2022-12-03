package heast.client.gui.components.welcome

import heast.client.gui.registry.Colors
import javafx.scene.control.Label
import javafx.scene.layout.VBox
import javafx.scene.paint.CycleMethod
import javafx.scene.paint.LinearGradient
import javafx.scene.paint.Stop

class Title : VBox() {
	init {
		this.styleClass.add("spacing-3")
		this.children.addAll(
			Label("Welcome to").apply {
				this.styleClass.addAll(
					"font-poppins-bold",
					"text-white",
					"text-fette")
			},

			Label("Heast Messenger").apply {
				this.styleClass.addAll(
					"font-poppins-bold",
					"text-fette")
				this.textFill = LinearGradient(
					0.0, 0.0, 1.0, 0.0, true, CycleMethod.NO_CYCLE,
					Stop(0.0, Colors.accent),
					Stop(1.0, Colors.accentBrighter)
				)
			},
		)
	}
}