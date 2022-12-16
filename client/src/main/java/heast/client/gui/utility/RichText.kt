package heast.client.gui.utility

import heast.client.gui.cssengine.CSSProperty
import heast.client.gui.cssengine.CSSProperty.Companion.css
import heast.client.gui.utility.TextExtension.toText
import javafx.scene.text.TextFlow

open class RichText(text: String, styleClasses: List<CSSProperty> = emptyList()) : TextFlow() {
	init {
		val finalText = mutableListOf<Pair<Char, String>>()
		val textParts = text.split("%")
		for (part in textParts.indices) {
			if (part > 0) {
				finalText.add(Pair(textParts[part][0], textParts[part].substring(1)))
			} else {
				finalText.add(Pair('r', textParts[part]))
			}
		}

		this.children.addAll(finalText.map { (type, text) ->
			text.toText().apply {
				this.css = styleClasses
				this.style = this.style.replace("-fx-font-weight:.*?;".toRegex(),
					"-fx-font-weight: ${ when (type) {
						'r' -> "normal"
						'b' -> "bold"
						'i' -> "italic"
						else -> "normal"
					} };")
			}
		})
	}
}