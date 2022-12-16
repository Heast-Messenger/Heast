package heast.client.gui.cssengine

import heast.client.gui.registry.Colors
import heast.client.gui.registry.Colors.toHex
import javafx.scene.paint.Color

class Font : CSSProperty() {
	enum class Size(private val size: Int) {
		XS(14),
		SMALL(18),
		MEDIUM(24),
		LARGE(30);
		override fun toString() = "${size}px"
	}

	enum class Weight(private val weight: String) {
		BOLD("bold"),
		REGULAR("normal"),
		ITALIC("italic");
		override fun toString() = weight
	}

	private var family : String = "Poppins"
	private var size : Size = Size.MEDIUM
	private var weight : Weight = Weight.REGULAR
	private var color : Color = Colors.WHITE
	private var promptColor : Color = Colors.SECONDARY

	fun family(value : String) = apply { this.family = value }
	fun size(value : Size) = apply { this.size = value }
	fun weight(value : Weight) = apply { this.weight = value }
	fun color(value : Color) = apply { this.color = value }
	fun promptColor(value : Color) = apply { this.promptColor = value }

	override fun toString() =
		"-fx-font-family: ${this.family};" +
		"-fx-font-size: ${this.size};" +
		"-fx-font-weight: ${this.weight};" +
		"-fx-text-fill: ${this.color.toHex()};" +
		"-fx-fill: ${this.color.toHex()};" +
		"-fx-prompt-text-fill: ${this.promptColor.toHex()};"
}