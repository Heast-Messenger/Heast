package heast.client.view

import javafx.beans.binding.Bindings
import javafx.geometry.Insets
import javafx.geometry.Pos
import javafx.scene.Node
import javafx.scene.image.Image
import javafx.scene.layout.*
import javafx.scene.paint.Color
import heast.client.control.network.ClientNetwork
import heast.client.model.Settings
import heast.client.view.dialog.Dialog
import heast.client.view.template.Button
import heast.client.view.template.TextField
import heast.client.view.utility.FlexExpander
import heast.client.view.utility.FlexItem
import heast.client.view.utility.FontManager

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

	object LoginPane : VBox() {
		private val emailField : TextField
		private val passwordField : TextField
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
						ClientNetwork.INSTANCE.reset(
							emailField.text, passwordField.text
						)
					}.apply {
						this.alignment = Pos.CENTER
					},

					Button("Log in", Color.web("#ECF0FF"), Image(
						"/heast/client/images/settings/login.png"
					)) {
						ClientNetwork.INSTANCE.login(
							emailField.text,
							passwordField.text
						)
					}.apply {
						this.alignment = Pos.CENTER
					},

					Button("Back", Color.web("#ECF0FF"), Image(
						"/heast/client/images/misc/back.png"
					)) {
						ClientGui.resize(450.0)
						setPane(WelcomePane)
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

	object SignupPane : VBox() {
		private val usernameField : TextField
		private val emailField : TextField
		private val passwordField : TextField

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
					)) {
						ClientNetwork.INSTANCE.signup(
							usernameField.text,
							emailField.text,
							passwordField.text
						)
					}.apply {
						this.alignment = Pos.CENTER
					},

					Button("Back", Color.web("#ECF0FF"), Image(
						"/heast/client/images/misc/back.png"
					)) {
						ClientGui.resize(450.0)
						setPane(WelcomePane)
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
						"/heast/client/images/settings/connected.png"
					)) {
						ClientNetwork.INSTANCE.verify(
							verificationField.text.uppercase()
						)
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
}