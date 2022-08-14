package heast.client.view.utility

import javafx.scene.paint.Color
import java.util.*

object ColorUtil {
	fun toHex(c: Color) : String {
		return String.format( "#%02X%02X%02X",
			(c.red * 255).toInt(),
			(c.green * 255).toInt(),
			(c.blue * 255).toInt()
		)
	}

	fun toRGB(c: Color) : String {
		return toRGBA(c, 1.0)
	}

	fun toRGBA(c: Color, o: Double) : String {
		return String.format(Locale.US, "rgba(%d, %d, %d, %f)",
			(c.red * 255).toInt(),
			(c.green * 255).toInt(),
			(c.blue * 255).toInt(),
			o
		)
	}

	fun toColor(hex: String) : Color {
		return if (hex.matches(Regex("#[\\da-fA-F]{6}")) || hex.matches(Regex("#[\\da-fA-F]{3}"))) {
			Color.web(hex)
		} else {
			Color.BLACK
		}
	}
}