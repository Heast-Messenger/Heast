package heast.client.gui.components.layout

import heast.client.gui.GuiMain
import javafx.scene.Parent
import kotlin.reflect.KClass

object Link {
	fun <T : Parent> T.linkTo(scene: KClass<out Parent>) : T {
		this.setOnMouseClicked {
			GuiMain.window.mantle.content = scene.objectInstance!!
		}
		return this@linkTo
	}
}