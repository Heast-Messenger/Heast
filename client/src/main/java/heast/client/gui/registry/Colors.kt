package heast.client.gui.registry

import javafx.scene.paint.Color

object Colors {
	val black = Color.web("#1E1E22")
	val white = Color.web("#EBF2FF")
	val primary = Color.web("#26262B")
	val secondary = Color.web("#545764")
	val border = Color.web("#34343A")
	val selected = Color.web("#545764")
	val accent = Color.web("#FEBC2E")
	val accentBrighter = Color.web("#FFCF8A")

	fun colors() = mapOf(
		"black" to black,
		"white" to white,
		"primary" to primary,
		"secondary" to secondary,
		"border" to border,
		"selected" to selected,
		"accent" to accent
	)

	fun Color.toHex() = String.format("#%02X%02X%02X", (red * 255).toInt(), (green * 255).toInt(), (blue * 255).toInt())
}