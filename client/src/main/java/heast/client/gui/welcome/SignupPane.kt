package heast.client.gui.welcome

import heast.client.gui.ClientGui
import heast.client.gui.WelcomeView
import heast.client.gui.template.LoadingPane.Companion.verificationLoader
import heast.client.gui.dialog.Dialog
import heast.client.gui.template.Button
import heast.client.gui.template.TextField
import heast.client.gui.utility.FlexExpander
import heast.client.gui.utility.FontManager
import heast.client.model.Settings
import heast.client.network.ClientNetwork
import javafx.beans.binding.Bindings
import javafx.geometry.Insets
import javafx.geometry.Pos
import javafx.scene.image.Image
import javafx.scene.layout.Background
import javafx.scene.layout.VBox
import javafx.scene.paint.Color

object SignupPane : VBox() {
	private val usernameField : TextField
	private val emailField : TextField
	private val passwordField : TextField

	private fun clearFields(){
		this.usernameField.clear()
		this.emailField.clear()
		this.passwordField.clear()
	}

	init {
		this.padding = Insets(30.0)
		this.spacing = 20.0
		this.backgroundProperty().bind(
			Bindings.createObjectBinding({
				Background.fill(Settings.colors["Secondary Color"]!!.color.value)
			}, Settings.colors["Secondary Color"]!!.color)
		)

		this.children.addAll(
			FontManager.boldLabel("Create a new account", 26.0).apply {
			},

			FontManager.regularLabel("Tell us your name, email address and choose a strong password.", 16.0).apply {
				this.opacity = 0.5
			},

			FontManager.regularLabel("You will be sent a verification code to your email address.", 16.0).apply {
				this.opacity = 0.5
			},

			FlexExpander(
				vBox = true
			),

			VBox(
				TextField("Username").apply {
					this@SignupPane.usernameField = this
				},

				TextField("Email address").apply {
					this@SignupPane.emailField = this
				},

				TextField("Password").apply {
					this@SignupPane.passwordField = this
				},

				Button("Sign up", Color.web("#B8FFB7"), Image(
					"/heast/client/images/settings/signup.png"
				)
				) {
					ClientNetwork.signup(
						usernameField.text.trim(),
						emailField.text.trim(),
						passwordField.text,
					) {
						Dialog.show(verificationLoader, WelcomeView)
						clearFields()
					}
				}.apply {
					this.alignment = Pos.CENTER
				},

				Button("Back", Color.web("#ECF0FF"), Image(
					"/heast/client/images/misc/back.png"
				)) {
					ClientGui.resize(450.0)
					WelcomeView.setPane(WelcomeView.WelcomePane)
					clearFields()
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