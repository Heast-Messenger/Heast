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

object LoginPane : VBox() {
	private val emailField : TextField
	private val passwordField : TextField

	private fun clearFields() {
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
			FontManager.boldLabel("Log into your account", 26.0),

			FontManager.regularLabel("Type in your email address and your password.", 16.0).apply {
				this.opacity = 0.5
			},

			FontManager.regularLabel("In case you forgot the password, you can reset it anytime. Just type in your new password into the password field.", 16.0).apply {
				this.opacity = 0.5
			},

			FlexExpander(
				vBox = true
			),

			VBox(
				TextField("Email address").apply {
					this@LoginPane.emailField = this
				},

				TextField("Password").apply {
					this@LoginPane.passwordField = this
				},

				Button("Reset your password", Color.web("#FFF176"), Image(
					"/heast/client/images/settings/resetpwd.png"
				)) {
					ClientNetwork.reset(
						emailField.text.trim(),
						passwordField.text
					) {
						Dialog.show(verificationLoader, WelcomeView)
						clearFields()
					}
				}.apply {
					this.alignment = Pos.CENTER
				},

				Button("Log in", Color.web("#ECF0FF"), Image(
					"/heast/client/images/settings/login.png"
				)) {
					ClientNetwork.login(
						emailField.text.trim(),
						passwordField.text
					) {
						Dialog.show(verificationLoader, WelcomeView)
						clearFields()
					}
				}.apply {
					this.alignment = Pos.CENTER
				},

				Button("Back", Color.web("#ECF0FF"), Image(
					"/heast/client/images/misc/back.png"
				)
				) {
					ClientGui.resize(450.0)
					clearFields()
					WelcomeView.setPane(WelcomeView.WelcomePane)
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