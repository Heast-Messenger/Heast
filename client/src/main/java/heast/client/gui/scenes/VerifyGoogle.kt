package heast.client.gui.scenes

import heast.client.gui.components.window.Header
import heast.client.gui.components.window.WindowHeight
import javafx.scene.Node

@WindowHeight(600)
object VerifyGoogle : VerifyBase() {
	override val title : Node
		get() = Header("Verify it's you",
			"Enter the code from your %bGoogle Authenticator")
}