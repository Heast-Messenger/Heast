package heast.client.gui.scenes

import heast.client.gui.components.layout.Button
import heast.client.gui.components.layout.Extender
import heast.client.gui.components.layout.VerifyField
import heast.client.gui.components.window.Default
import heast.client.gui.components.window.WindowHeight
import heast.client.gui.cssengine.Align
import heast.client.gui.cssengine.CSSProperty.Companion.css
import heast.client.gui.cssengine.Padding
import heast.client.gui.cssengine.Spacing
import heast.client.gui.registry.Icons
import javafx.scene.Node
import javafx.scene.Parent
import javafx.scene.layout.HBox
import javafx.scene.layout.VBox
import kotlin.reflect.KClass

@WindowHeight(600)
abstract class VerifyBase : Default() {
	override val back : KClass<out Parent>
		get() = Welcome::class

	override val forward : KClass<out Parent>?
		get() = null

	abstract override val title : Node

	override val layout : Node
		get() = VBox().apply {
			this.children.add(
				Extender.vbox())

			this.children.add(
				HBox().apply {
					val fields = arrayOfNulls<VerifyField>(5)
					(4 downTo 0).map {
						fields[it] = VerifyField(it, fields)
					}

					fields.forEach { field ->
						this.children.add(
							field)
					}

					this.css = listOf(
						Align.center,
						Spacing.`2`)
				})

			this.children.add(
				Extender.vbox())

			this.children.add(
				Button.builder()
					.withText("Verify")
					.withIcon(Icons.Verify.VERIFY)
					.onClick { println("Verifying using email") }
					.build())

			this.css = listOf(
				Align.center,
				Spacing.`4`,
				Padding().y(4))
		}
}