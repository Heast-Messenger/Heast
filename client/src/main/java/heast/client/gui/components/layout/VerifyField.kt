package heast.client.gui.components.layout

import heast.client.gui.cssengine.Align
import heast.client.gui.cssengine.CSSProperty.Companion.css
import heast.client.gui.cssengine.Font
import heast.client.gui.registry.Colors
import heast.client.gui.registry.Colors.toHex
import heast.client.gui.registry.Settings
import javafx.event.EventHandler
import javafx.scene.control.TextField
import javafx.scene.input.KeyCode
import javafx.scene.layout.Border
import org.apache.commons.lang3.StringUtils

class VerifyField(private val index : Int, private val fields : Array<VerifyField?>) : TextField() {
	init {
		this.css = listOf(
			Align.center,
			Font()
				.family("Poppins")
				.weight(Font.Weight.BOLD)
				.color(Colors.WHITE)
				.size(Font.Size.LARGE),
		)

		this.textProperty().addListener { _, oldValue, newValue ->
			// If the string contains invalid characters, remove them
			this.text = this.text
				.uppercase()
				.replace("[^${Settings.Verification.ALLOWED_CHARS}]".toRegex(), "")

			// If nothing was changed, return
			if (this.text.equals(oldValue)) {
				return@addListener
			}

			// If the string is too long, use the first added character
			if (newValue.length > 1) {
				this.text = StringUtils.difference(oldValue, newValue)[0].toString()
			}

			// If the input was removed, move the cursor to the previous field
			if (index > 0 && newValue.isEmpty()) {
				fields[index-1]!!.requestFocus()
			}

			// If input was added, move the cursor to the next field
			if (index < fields.size-1 && newValue.isNotEmpty()) {
				fields[index+1]!!.requestFocus()
			}
		}

		this.onKeyPressed = EventHandler {
			// Special handling for backspace when the field is empty
			if (it.code == KeyCode.BACK_SPACE || it.code == KeyCode.DELETE) {
				if (index > 0 && this.text.isEmpty()) {
					fields[index-1]!!.requestFocus()
				}
			}
		}

		this.border = Border.EMPTY
		this.style +=
			"-fx-border-color: ${Colors.BORDER.toHex()};" +
			"-fx-border-width: 0 0 3 0;" +
			"-fx-padding: 0 0 0 0;"
	}
}