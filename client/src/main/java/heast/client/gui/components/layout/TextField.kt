package heast.client.gui.components.layout

import heast.client.gui.cssengine.*
import heast.client.gui.cssengine.CSSProperty.Companion.css
import heast.client.gui.registry.Colors
import javafx.scene.control.TextField as JFXTextField

class TextField(prompt: String, action : (input: String) -> Unit) : JFXTextField() {
	class TextFieldBuilder {
		private var prompt : String = "TextField"
		private var action : (input: String) -> Unit = {}

		fun withPrompt(prompt : String) = apply { this.prompt = prompt }
		fun onType(action : (input: String) -> Unit) = apply { this.action = action }

		fun build() : TextField {
			return TextField(prompt, action)
		}
	}

	companion object {
		fun builder() = TextFieldBuilder()
	}

	init {
		this.promptText = prompt

		this.css = listOf(
			Align.centerLeft,
			Spacing.`3`,
			Cursor.text,
			Padding().all(4),
			Font()
				.family("Poppins")
				.weight(Font.Weight.BOLD)
				.size(Font.Size.SMALL)
				.color(Colors.WHITE)
				.promptColor(Colors.SECONDARY),
			Pane()
				.colorBG(Colors.BLACK)
				.colorBD(Colors.BORDER)
				.roundAll(4))

		this.textProperty().addListener { _, _, newValue ->
			action(newValue) }
	}
}