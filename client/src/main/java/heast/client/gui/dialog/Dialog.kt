package heast.client.gui.dialog

import heast.client.model.Settings
import heast.client.gui.utility.ColorUtil
import heast.client.gui.utility.MultiStack
import javafx.beans.binding.Bindings
import javafx.geometry.Insets
import javafx.scene.Node
import javafx.scene.layout.*
import javafx.scene.paint.Color

object Dialog {
	fun show(content: Node, parent: StackPane) {
		MultiStack.addStackPaneView(
			BackGround, parent, scale = false
		)
		MultiStack.addStackPaneView(
			content, parent
		)
	}

	fun close(content: Node, parent: StackPane) {
		if (parent.children.contains(content)) {
			if (parent.children.size <= 3) {
				MultiStack.removeStackPaneView(BackGround, parent, scale = false)
			}
			MultiStack.removeStackPaneView(content, parent, fade = false)
		}
	}

	object BackGround : Pane() {
		init {
			this.backgroundProperty().bind(
				Bindings.createObjectBinding({
					Background(
						BackgroundFill(
							Color.web(
								ColorUtil.toRGBA(
								Settings.colors["Primary Color"]!!.color.value, 0.8
							) ),
							CornerRadii(10.0),
							Insets.EMPTY
						)
					)
				}, Settings.colors["Primary Color"]!!.color)
			)
		}
	}
}