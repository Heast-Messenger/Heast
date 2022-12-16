package heast.client.gui.scenes

import heast.client.gui.components.layout.VerticalButton
import heast.client.gui.components.window.Default
import heast.client.gui.components.window.Header
import heast.client.gui.components.window.WindowHeight
import heast.client.gui.cssengine.Align
import heast.client.gui.cssengine.CSSProperty.Companion.css
import heast.client.gui.cssengine.Padding
import heast.client.gui.cssengine.Spacing
import heast.client.gui.registry.Icons
import javafx.scene.Node
import javafx.scene.Parent
import javafx.scene.layout.HBox
import kotlin.reflect.KClass

@WindowHeight(600)
object Method : Default() {

	override val back : KClass<out Parent>
		get() = Welcome::class

	override val forward : KClass<out Parent>?
		get() = null

	override val title : Node
		get() = Header("Choose method",
			"Either verify through the %bGoogle Authenticator %rapp, " +
					"or via a code sent your %bemail %raddress")

	override val layout : Node
		get() = HBox().apply {
			this.children.add(
				VerticalButton.builder()
					.withText("Email")
					.withIcon(Icons.Verify.EMAIL)
					.onClick { println("Verifying via email") }
					.build())

			this.children.add(
				VerticalButton.builder()
					.withText("Google")
					.withIcon(Icons.Verify.GOOGLE)
					.onClick { println("Verifying via Google Authenticator") }
					.build())

			this.css = listOf(
				Align.center,
				Spacing.`4`,
				Padding().y(16))
		}
}