package heast.client.gui.components.window

import heast.client.gui.GuiMain
import heast.client.gui.cssengine.Align
import heast.client.gui.cssengine.CSSProperty.Companion.css
import heast.client.gui.cssengine.Cursor
import heast.client.gui.cssengine.Padding
import heast.client.gui.registry.Icons.toImg
import heast.client.gui.registry.Interpolators
import heast.client.gui.utility.Transition
import javafx.animation.RotateTransition
import javafx.event.EventHandler
import javafx.scene.Parent
import javafx.scene.image.ImageView
import javafx.scene.layout.StackPane
import javafx.util.Duration
import kotlin.reflect.KClass

class Navigator(img : String, target : KClass<out Parent>?) : StackPane() {
	private val icon: ImageView

	init {
		this.css = listOf(
			Align.center,
			Cursor.pointer,
			Padding().y(4))

		this.children.addAll(
			ImageView(img.toImg()).apply {
				this.fitWidth = 28.0
				this.fitHeight = 28.0
				icon = this
			})

		this.onMouseClicked = EventHandler {
			if (target != null) {
				GuiMain.window.mantle.content = target.objectInstance!!
			}
		}

		this.onMouseEntered = EventHandler {
			Transition {
				icon.scaleX = it * 0.2 + 1.0
				icon.scaleY = it * 0.2 + 1.0
			}.apply {
				this.duration = Duration.millis(200.0)
				this.play() }

			RotateTransition().apply {
				this.node = icon
				this.duration = Duration.millis(1000.0)
				this.fromAngle = 0.0
				this.toAngle = 20.0
				this.interpolator = Interpolators.NAVIGATOR
				this.play() }
		}

		this.onMouseExited = EventHandler {
			Transition {
				icon.scaleX = (1.0 - it) * 0.2 + 1.0
				icon.scaleY = (1.0 - it) * 0.2 + 1.0
			}.apply {
				this.duration = Duration.millis(200.0)
				this.play() }
		}
	}
}