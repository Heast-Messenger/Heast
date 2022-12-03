package heast.client.gui.scenes

import heast.client.gui.components.welcome.Default
import javafx.scene.Parent
import kotlin.reflect.KClass

object Welcome : Default() {
	init {

	}

	override val back : KClass<out Parent>
		get() = Start::class
}