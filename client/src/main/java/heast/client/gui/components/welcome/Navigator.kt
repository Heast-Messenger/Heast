package heast.client.gui.components.welcome

import heast.client.gui.GuiMain
import heast.client.gui.registry.Icons.toImg
import heast.client.gui.utility.Transition
import javafx.animation.Interpolator
import javafx.animation.RotateTransition
import javafx.event.EventHandler
import javafx.scene.Parent
import javafx.scene.image.ImageView
import javafx.scene.layout.StackPane
import javafx.util.Duration
import kotlin.math.pow
import kotlin.math.sin
import kotlin.random.Random
import kotlin.reflect.KClass

class Navigator(img : String, target : KClass<out Parent>?) : StackPane() {
	private val icon: ImageView

	init {
		println("Navigator: $target")
		this.styleClass.addAll(
			"py-4",
			"align-center")

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
				this.toAngle = 50.0

				val randomDirection = Random.nextInt(0, 2) * 2 - 1
				this.interpolator = object : Interpolator() {
					override fun curve(t : Double) = randomDirection * sin(10*t)*2.0.pow(-5*t)
				}
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