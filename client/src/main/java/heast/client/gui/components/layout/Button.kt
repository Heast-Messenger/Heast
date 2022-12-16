package heast.client.gui.components.layout

import heast.client.gui.cssengine.*
import heast.client.gui.cssengine.CSSProperty.Companion.css
import heast.client.gui.registry.Colors
import heast.client.gui.registry.Icons.toImg
import heast.client.gui.utility.TextExtension.toText
import javafx.scene.image.ImageView
import javafx.scene.layout.HBox
import javafx.scene.layout.VBox

class Button(text: String, icon: String?, action: () -> Unit, isDisabled: Boolean) : HBox() {
	class ButtonBuilder {
		private var text : String = "Button"
		private var icon : String? = null
		private var action : () -> Unit = {}
		private var isDisabled : Boolean = false

		fun withText(text : String) = apply { this.text = text }
		fun withIcon(icon : String) = apply { this.icon = icon }
		fun onClick(action : () -> Unit) = apply { this.action = action }
		fun isDisabled(isDisabled : Boolean) = apply { this.isDisabled = isDisabled }

		fun build() : Button {
			return Button(text, icon, action, isDisabled)
		}
	}

	companion object {
		fun builder() = ButtonBuilder()
	}

	init {
		this.children.addAll(
			ImageView(icon?.toImg()).apply {
				this.fitWidth = 26.0
				this.fitHeight = 26.0
			},

			text.toText().apply {
				this.css = listOf(
					Font()
						.family("Poppins")
						.weight(Font.Weight.BOLD)
						.size(Font.Size.SMALL)
						.color(Colors.WHITE)
				)
			})

		this.css = listOf(
			Align.center,
			Spacing.`3`,
			Cursor.pointer,
			Padding().all(4),
			Pane()
				.colorBG(Colors.BLACK)
				.colorBD(Colors.BORDER)
				.roundAll(4))

		this.setOnMouseClicked {
			if (!isDisabled) {
				action()
			}
		}
	}
}

class VerticalButton(text: String, icon: String?, action: () -> Unit, isDisabled: Boolean) : VBox() {
	class ButtonBuilder {
		private var text : String = "Button"
		private var icon : String? = null
		private var action : () -> Unit = {}
		private var isDisabled : Boolean = false

		fun withText(text : String) = apply { this.text = text }
		fun withIcon(icon : String) = apply { this.icon = icon }
		fun onClick(action : () -> Unit) = apply { this.action = action }
		fun isDisabled(isDisabled : Boolean) = apply { this.isDisabled = isDisabled }

		fun build() : VerticalButton {
			return VerticalButton(text, icon, action, isDisabled)
		}
	}

	companion object {
		fun builder() = ButtonBuilder()
	}

	init {
		this.children.addAll(
			ImageView(icon?.toImg()).apply {
				this.fitWidth = 26.0
				this.fitHeight = 26.0
			},

			text.toText().apply {
				this.css = listOf(
					Font()
						.family("Poppins")
						.weight(Font.Weight.BOLD)
						.size(Font.Size.SMALL)
						.color(Colors.WHITE)
				)
			})

		this.css = listOf(
			Align.center,
			Spacing.`3`,
			Cursor.pointer,
			Padding().all(4),
			Pane()
				.colorBG(Colors.BLACK)
				.colorBD(Colors.BORDER)
				.roundAll(4))

		this.prefWidth = 150.0
		this.prefHeight = 200.0

		this.setOnMouseClicked {
			if (!isDisabled) {
				action()
			}
		}
	}
}