package heast.client.gui.template

import heast.client.gui.WelcomeView
import heast.client.gui.dialog.Dialog
import heast.client.gui.utility.FlexExpander
import heast.client.gui.utility.FontManager
import heast.client.model.Settings
import heast.client.network.ClientNetwork
import javafx.animation.Interpolator
import javafx.animation.RotateTransition
import javafx.beans.binding.Bindings
import javafx.geometry.Insets
import javafx.geometry.Pos
import javafx.scene.image.Image
import javafx.scene.image.ImageView
import javafx.scene.layout.Background
import javafx.scene.layout.BackgroundFill
import javafx.scene.layout.CornerRadii
import javafx.scene.layout.HBox
import javafx.scene.layout.VBox
import javafx.util.Duration

class LoadingPane(title: String, vararg reasons: String) : VBox() {
	companion object {
		val verificationLoader = LoadingPane(
			"Your email is on its way",
			"Check your inbox as your email will arrive in any moment.",
			"If you did not receive an email, you can request a new one with the button below, or try with a different email address.",
			"You can cancel the process at any time."
		)
	}

	private val emailField : TextField

	init {
		this.padding = Insets(50.0)
		this.spacing = 20.0
		this.isFillWidth = true
		this.alignment = Pos.TOP_CENTER
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

		this.children.add(
			FontManager.boldLabel(title, 26.0),
		)

		for (reason in reasons) {
			this.children.add(
				FontManager.regularLabel(reason, 16.0).apply {
					this.opacity = 0.5
				}
			)
		}

		this.children.addAll(
			FlexExpander(
				vBox = true
			),

			ImageView(
				Image("/heast/client/images/misc/loading.png")
			).apply {
				this.fitWidth = 50.0
				this.fitHeight = 50.0
			}.also { iv ->
				RotateTransition().apply {
					this.node = iv
					this.duration = Duration.seconds(1.0)
					this.cycleCount = RotateTransition.INDEFINITE
					this.interpolator = Interpolator.LINEAR
					this.byAngle = 360.0
				}.play()
			},

			FlexExpander(
				vBox = true
			),

			HBox(
				TextField("Email address").apply {
					this@LoadingPane.emailField = this
				},
				Button(icon = Image(
					"/heast/client/images/misc/confirm.png"
				)) {
					ClientNetwork.resend(
						emailField.text.trim()
					)
				}.apply {
					this.alignment = Pos.CENTER
				},
				Button(icon = Image(
					"/heast/client/images/misc/back.png"
				)) {
					ClientNetwork.cancel()
					Dialog.close(this, WelcomeView)
				}.apply {
					this.alignment = Pos.CENTER
				}
			).apply {
				this.alignment = Pos.CENTER
				this.spacing = 10.0
			},
		)
	}
}