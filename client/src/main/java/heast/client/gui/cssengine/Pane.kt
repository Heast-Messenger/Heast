package heast.client.gui.cssengine

import heast.client.gui.registry.Colors.toHex
import javafx.scene.paint.Color

class Pane : CSSProperty() {
	private var colorBG : Color = Color.TRANSPARENT
	private var colorBD : Color = Color.TRANSPARENT
	private var roundTopLeft : Int = 0
	private var roundTopRight : Int = 0
	private var roundBottomLeft : Int = 0
	private var roundBottomRight : Int = 0

	fun colorBG(value : Color) = apply { this.colorBG = value }
	fun colorBD(value : Color) = apply { this.colorBD = value }
	fun roundTopLeft(value : Int) = apply { this.roundTopLeft = MULTIPLIER * value }
	fun roundTopRight(value : Int) = apply { this.roundTopRight = MULTIPLIER * value }
	fun roundBottomLeft(value : Int) = apply { this.roundBottomLeft = MULTIPLIER * value }
	fun roundBottomRight(value : Int) = apply { this.roundBottomRight = MULTIPLIER * value }
	fun roundAll(value : Int) = apply { roundTopLeft(value); roundTopRight(value); roundBottomLeft(value); roundBottomRight(value) }

	override fun toString() =
		"-fx-background-color: ${this.colorBG.toHex()};" +
		"-fx-border-color: ${this.colorBD.toHex()};" +
		"-fx-border-radius: ${this.roundTopLeft}px ${this.roundTopRight}px ${this.roundBottomRight}px ${this.roundBottomLeft}px;" +
		"-fx-background-radius: ${this.roundTopLeft}px ${this.roundTopRight}px ${this.roundBottomRight}px ${this.roundBottomLeft}px;"
}