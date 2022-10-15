package heast.client.gui

import javafx.beans.binding.Bindings
import javafx.geometry.Insets
import javafx.geometry.Pos
import javafx.scene.Node
import javafx.scene.image.Image
import javafx.scene.layout.*
import javafx.scene.paint.Color
import heast.client.model.Settings
import heast.client.gui.template.Button
import heast.client.gui.utility.FlexExpander
import heast.client.gui.utility.FontManager
import heast.client.gui.welcome.LoginPane
import heast.client.gui.welcome.SignupPane

object WelcomeView : StackPane() {
	init {
		setPane(WelcomePane)
	}

	fun setPane(node: Node) {
		this.children.clear()
		this.children.add(node)
	}

	object WelcomePane : VBox() {
		init {
			this.padding = Insets(30.0)
			this.spacing = 20.0
			this.backgroundProperty().bind(
				Bindings.createObjectBinding({
					Background.fill(Settings.colors["Secondary Color"]!!.color.value)
				}, Settings.colors["Secondary Color"]!!.color)
			)

			this.children.addAll(
				FontManager.boldLabel("Welcome to Heast Messenger!", 26.0),

				FontManager.regularLabel("The messenger with revolutionary technology", 16.0).apply {
					this.opacity = 0.5
				},

				FlexExpander(
					vBox = true
				),

				VBox(
					Button("Log in", Color.web("#ECF0FF"), Image(
						"/heast/client/images/settings/login.png"
					)) {
						ClientGui.resize(700.0)
						setPane(LoginPane)
					}.apply {
						this.alignment = Pos.CENTER
					},

					Button("Sign up", Color.web("#B8FFB7"), Image(
						"/heast/client/images/settings/signup.png"
					)) {
						ClientGui.resize(650.0)
						setPane(SignupPane)
					}.apply {
						this.alignment = Pos.CENTER
					}
				).apply {
					this.padding = Insets(30.0, 50.0, 30.0, 50.0)
					this.spacing = 22.0
				}
			)
		}
	}
}