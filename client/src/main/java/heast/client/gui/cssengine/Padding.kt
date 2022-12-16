package heast.client.gui.cssengine

class Padding : CSSProperty() {
	private var top : Int = 0
	private var right : Int = 0
	private var bottom : Int = 0
	private var left : Int = 0

	fun top(value : Int) = apply { this.top = MULTIPLIER * value }
	fun right(value : Int) = apply { this.right = MULTIPLIER * value }
	fun bottom(value : Int) = apply { this.bottom = MULTIPLIER * value }
	fun left(value : Int) = apply { this.left = MULTIPLIER * value }
	fun x(value : Int) = apply { left(value); right(value) }
	fun y(value : Int) = apply { top(value); bottom(value) }
	fun all(value : Int) = apply { x(value); y(value) }

	fun build() = toString()

	override fun toString() =
		"-fx-padding: ${this.top}px ${this.right}px ${this.bottom}px ${this.left}px;"
}