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
object Signup : Default() {
	override val back : KClass<out Parent>
		get() = Welcome::class

	override val forward : KClass<out Parent>?
		get() = null

	override val title : Node
		get() = Header("Welcome!",
			"Tell us your %bname%r, %bemail address%r, and choose a %bstrong password")

	override val layout : Node
		get() = VBox().apply {
			this.children.addAll(
				TextField.builder()
					.withPrompt("Username")
					.build(),

				TextField.builder()
					.withPrompt("Email address")
					.build(),

				TextField.builder()
					.withPrompt("Password")
					.build())

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
							.withText("Sign up")
							.withIcon(Icons.Menu.SIGNUP)
							.onClick { println("Signing up") }
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