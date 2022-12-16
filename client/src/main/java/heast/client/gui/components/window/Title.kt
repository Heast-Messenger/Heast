package heast.client.gui.components.window

import heast.client.gui.cssengine.CSSProperty.Companion.css
import heast.client.gui.cssengine.CSSProperty.Companion.removeCSSProperty
import heast.client.gui.cssengine.Font
import heast.client.gui.cssengine.Spacing
import heast.client.gui.registry.Colors
import heast.client.gui.utility.TextExtension.toText
import javafx.scene.layout.VBox
import javafx.scene.paint.CycleMethod
import javafx.scene.paint.LinearGradient
import javafx.scene.paint.Stop

class Title : VBox() {
	init {
		this.children.addAll(
			"Welcome to".toText().apply {
				this.css = listOf(
					Font()
						.family("Poppins")
						.weight(Font.Weight.BOLD)
						.color(Colors.WHITE)
						.size(Font.Size.LARGE))
			},

			"Heast Messenger".toText().apply {
				this.css = listOf(
					Font()
						.family("Poppins")
						.weight(Font.Weight.BOLD)
						.size(Font.Size.LARGE))

				this.removeCSSProperty("-fx-text-fill", "-fx-fill")
				this.fill = LinearGradient(
					0.0, 0.0, 1.0, 0.0, true, CycleMethod.NO_CYCLE,
					Stop(0.0, Colors.ACCENT),
					Stop(1.0, Colors.ACCENT_BRIGHTER)
				)
			},
		)

		this.css = listOf(
			Spacing.`3`)
	}
}