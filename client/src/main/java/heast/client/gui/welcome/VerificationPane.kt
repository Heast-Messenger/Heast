package heast.client.gui.welcome

import heast.client.gui.WelcomeView
import heast.client.gui.dialog.Dialog
import heast.client.gui.template.Button
import heast.client.gui.template.TextField
import heast.client.gui.utility.FlexExpander
import heast.client.gui.utility.FlexItem
import heast.client.gui.utility.FontManager
import heast.client.model.Settings
import heast.client.network.ClientNetwork
import javafx.beans.binding.Bindings
import javafx.geometry.Insets
import javafx.geometry.Pos
import javafx.scene.image.Image
import javafx.scene.layout.*
import javafx.scene.paint.Color

object VerificationPane : VBox() {
	private val verificationField : TextField

	init {
		this.backgroundProperty().bind(
			Bindings.createObjectBinding({
				Background(
					BackgroundFill(
						Settings.colors["Secondary Color"]!!.color.value,
						CornerRadii(10.0),
						Insets(30.0)
					)
				)
			}, Settings.colors["Secondary Color"]!!.color)
		)

		this.padding = Insets(50.0)
		this.spacing = 20.0
		this.alignment = Pos.CENTER
		this.children.addAll(
			FlexExpander(
				vBox = true
			),

			FontManager.boldLabel("Verify your account", 26.0),

			FontManager.regularLabel("A verification code was just sent to your email address.", 16.0).apply {
				this.opacity = 0.5
			},

			FontManager.regularLabel("Type it in and click continue if you're ready.", 16.0).apply {
				this.opacity = 0.5
			},

			FlexItem(
				vBox = true
			),

			HBox(
				TextField("Verification Code").apply {
					this@VerificationPane.verificationField = this
				},

				Button(icon = Image(
					"/heast/client/images/misc/confirm.png"
				)) {
					ClientNetwork.verify(
						verificationField.text.uppercase()
					)
					verificationField.clear()
				}.apply {
					this.alignment = Pos.CENTER
				}
			).apply {
				this.alignment = Pos.CENTER
				this.spacing = 10.0
			},

			FlexExpander(
				vBox = true
			),

			Button("Back", Color.web("#ECF0FF"), Image(
				"/heast/client/images/misc/back.png"
			)) {
				Dialog.close(this@VerificationPane, WelcomeView)
			}.apply {
				this.alignment = Pos.CENTER
			}
		)
	}
}