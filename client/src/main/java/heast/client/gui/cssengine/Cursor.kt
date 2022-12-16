package heast.client.gui.cssengine

class Cursor(private val cursor: String) : CSSProperty() {
	companion object {
		val pointer = Cursor("hand")
		val default = Cursor("default")
		val wait = Cursor("wait")
		val text = Cursor("text")
	}

	override fun toString() =
		"cursor: ${this.cursor};"
}