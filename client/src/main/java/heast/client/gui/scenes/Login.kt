package heast.client.gui.scenes

import heast.client.gui.components.layout.Button
import heast.client.gui.components.layout.Extender
import heast.client.gui.components.layout.Link.linkTo
import heast.client.gui.components.layout.TextField
import heast.client.gui.components.window.Default
import heast.client.gui.components.window.Header
import heast.client.gui.components.window.WindowHeight
import heast.client.gui.cssengine.Align
import heast.client.gui.cssengine.CSSProperty.Companion.css
import heast.client.gui.cssengine.Font
import heast.client.gui.cssengine.Padding
import heast.client.gui.cssengine.Spacing
import heast.client.gui.registry.Colors
import heast.client.gui.registry.Icons
import heast.client.gui.utility.TextExtension.toText
import javafx.scene.Node
import javafx.scene.Parent
import javafx.scene.layout.VBox
import kotlin.reflect.KClass

@WindowHeight(630)
object Login : Default() {
	override val back : KClass<out Parent>
		get() = Welcome::class

	override val forward : KClass<out Parent>?
		get() = null

	override val title : Node
		get() = Header("Welcome back!",
			"Click on %bReset %rto change your %bpassword %rin case you forgot it.")

	override val layout : Node
		get() = VBox().apply {
			this.children.addAll(
				TextField.builder()
					.withPrompt("Username / Email")
					.build(),

				TextField.builder()
					.withPrompt("Password")
					.build(),

				Button.builder()
					.withText("Reset")
					.withIcon(Icons.Menu.RESET)
					.onClick { println("Resetting") }
					.build()
					.linkTo(Method::class))

			this.children.add(
				Extender.vbox())

			this.children.add(
				VBox().apply {
					this.children.add(
						"Ready to chat?".toText().apply {
							this.css = listOf(
								Font()
									.weight(Font.Weight.BOLD)
									.color(Colors.SECONDARY)
									.size(Font.Size.SMALL)
							)
						})

					this.children.add(
						Button.builder()
							.withText("Login")
							.withIcon(Icons.Menu.LOGIN)
							.onClick { println("Logging in") }
							.build()
							.linkTo(Method::class))

					this.css = listOf(
						Align.centerLeft,
						Spacing.`1`)
				})

			this.css = listOf(
				Align.center,
				Spacing.`4`,
				Padding().y(4))
		}
}