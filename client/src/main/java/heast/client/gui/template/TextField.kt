package heast.client.gui.template

import javafx.beans.binding.Bindings
import javafx.geometry.Insets
import javafx.geometry.Pos
import javafx.scene.Cursor
import javafx.scene.layout.Background
import javafx.scene.layout.BackgroundFill
import javafx.scene.layout.CornerRadii
import heast.client.model.Settings
import heast.client.gui.utility.ColorUtil
import heast.client.gui.utility.FontManager

open class TextField(prompt: String = "") : javafx.scene.control.TextField() {
	init {
		this.cursor = Cursor.TEXT
		this.prefHeight = 40.0
		this.padding = Insets(10.0)
		this.alignment = Pos.CENTER_LEFT
		this.font = FontManager.boldFont(16.0)
		this.promptText = prompt
		this.styleProperty().bind(
			Bindings.createObjectBinding({
				return@createObjectBinding "-fx-text-fill: ${
					ColorUtil.toHex(
						Settings.colors["Font Color"]!!.color.value
					)
				}; -fx-prompt-text-fill: ${
					ColorUtil.toRGBA(
						Settings.colors["Font Color"]!!.color.value, 0.5
					)
				};"
			}, Settings.colors["Font Color"]!!.color)
		)
		this.backgroundProperty().bind(
			Bindings.createObjectBinding({
				return@createObjectBinding Background(
					BackgroundFill(
						Settings.colors["Primary Color"]!!.color.value,
						CornerRadii(5.0),
						Insets.EMPTY
					)
				)
			}, Settings.colors["Primary Color"]!!.color)
		)
	}
}