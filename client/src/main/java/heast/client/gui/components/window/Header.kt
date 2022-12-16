package heast.client.gui.components.window

import heast.client.gui.cssengine.Align
import heast.client.gui.cssengine.CSSProperty.Companion.css
import heast.client.gui.cssengine.Font
import heast.client.gui.cssengine.Spacing
import heast.client.gui.registry.Colors
import heast.client.gui.utility.RichText
import heast.client.gui.utility.TextExtension.toText
import javafx.scene.layout.VBox

class Header(title: String, text: String) : VBox() {
	init {
		this.children.add(
			title.toText().apply {
				this.css = listOf(
					Font()
						.family("Poppins")
						.weight(Font.Weight.BOLD)
						.color(Colors.WHITE)
						.size(Font.Size.LARGE)
				)
			})

		this.children.add(
			RichText(text, listOf(
				Font()
					.family("Poppins")
					.color(Colors.SECONDARY)
					.size(Font.Size.SMALL)
			))
		)

		this.css = listOf(
			Align.centerLeft,
			Spacing.`2`)
	}
}