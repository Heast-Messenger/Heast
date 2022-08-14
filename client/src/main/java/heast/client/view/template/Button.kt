package heast.client.view.template

import javafx.animation.Interpolator
import javafx.animation.TranslateTransition
import javafx.beans.binding.Bindings
import javafx.beans.property.SimpleObjectProperty
import javafx.event.EventHandler
import javafx.geometry.Insets
import javafx.geometry.Pos
import javafx.scene.Cursor
import javafx.scene.image.Image
import javafx.scene.image.ImageView
import javafx.scene.input.MouseEvent
import javafx.scene.layout.*
import javafx.scene.paint.Color
import javafx.util.Duration
import heast.client.model.Settings
import heast.client.view.utility.AnyTransition
import heast.client.view.utility.FontManager
import heast.client.view.utility.HoverTransition

open class Button(text: String? = null, color: SimpleObjectProperty<Color>? = null, icon: Image? = null, onAction: EventHandler<MouseEvent>) : HBox() {
	constructor(text: String?, color: Color?, icon: Image?, onAction: EventHandler<MouseEvent>) : this(text, SimpleObjectProperty(color), icon, onAction)

	init {
		this.cursor = Cursor.HAND
		this.padding = Insets(10.0)
		this.spacing = 10.0
		this.prefHeight = 40.0
		this.alignment = Pos.CENTER_LEFT
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

		if (icon != null) {
			this.children.add(
				ImageView(icon).apply {
					this.fitWidth = 20.0
					this.fitHeight = 20.0
				}
			)
		}

		if (text != null) {
			this.children.add(
				FontManager.boldLabel(text, 16.0).apply {
					if (color != null) {
						this.textFillProperty().unbind()
						this.textFillProperty().bind(color)
					}
				}
			)
		}

		this.onMouseClicked = onAction

		this.onMouseEntered = EventHandler {
			HoverTransition.onMouseEntered(this@Button)
		}

		this.onMouseExited = EventHandler {
			HoverTransition.onMouseExited(this@Button)
		}
	}
}