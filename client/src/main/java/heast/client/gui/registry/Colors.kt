package heast.client.gui.registry

import javafx.scene.paint.Color
import java.awt.Color as AWTColor

object Colors {
	val BLACK = Color.web("#1E1E22")
	val WHITE = Color.web("#EBF2FF")
	val PRIMARY = Color.web("#26262B")
	val SECONDARY = Color.web("#545764")
	val BORDER = Color.web("#34343A")
	val SELECTED = Color.web("#545764")
	val ACCENT = Color.web("#FDB242")
	val ACCENT_BRIGHTER = Color.web("#FFCF8A")

	fun Color.toHex() = String.format("#%02X%02X%02X",
		(red * 255).toInt(), (green * 255).toInt(), (blue * 255).toInt())

	fun Color.toAWT(): AWTColor {
		return java.awt.Color(
			this.red.toFloat(),
			this.green.toFloat(),
			this.blue.toFloat(),
			this.opacity.toFloat())
	}
}