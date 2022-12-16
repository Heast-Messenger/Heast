package heast.client.gui.cssengine

class Spacing(private val value: Int) : CSSProperty() {
	companion object {
		val `1` = Spacing(MULTIPLIER * 1)
		val `2` = Spacing(MULTIPLIER * 2)
		val `3` = Spacing(MULTIPLIER * 3)
		val `4` = Spacing(MULTIPLIER * 4)
		val `6` = Spacing(MULTIPLIER * 6)
		val `8` = Spacing(MULTIPLIER * 8)
		val `12` = Spacing(MULTIPLIER * 12)
		val `16` = Spacing(MULTIPLIER * 16)
	}

	override fun toString() =
		"-fx-spacing: ${value}px;"
}