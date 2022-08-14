package heast.client.view.settings

import javafx.beans.binding.Bindings
import javafx.geometry.Insets
import javafx.geometry.Orientation
import javafx.geometry.Pos
import javafx.scene.control.Separator
import javafx.scene.image.Image
import javafx.scene.image.ImageView
import javafx.scene.layout.*
import javafx.scene.paint.Color
import heast.client.model.Settings
import heast.client.view.SettingsView
import heast.client.view.utility.FontManager

object NetworkArea : VBox() {
    init {
        this.isFillWidth = true
        this.spacing = 10.0
        this.children.addAll(
            SettingsView.SettingsGroup("Network"),
            HBox(
                VBox(
                    ImageView(
                        Image("/heast/client/images/settings/heast.png")
                    ).apply {
                        this.fitWidth = 50.0
                        this.fitHeight = 50.0
                    },
                    FontManager.boldLabel("Heast Authentication")
                ).apply {
                    this.alignment = Pos.CENTER
                    this.spacing = 5.0
                },

                Separator(Orientation.HORIZONTAL).apply {
                    this.background = Background.fill(Color.ALICEBLUE)
                    HBox.setHgrow(this, Priority.ALWAYS)
                },

                ImageView(
                    Image("/heast/client/images/settings/connected.png")
                ).apply {
                    this.fitWidth = 25.0
                    this.fitHeight = 25.0
                },

                Separator(Orientation.HORIZONTAL).apply {
                    this.background = Background.fill(Color.ALICEBLUE)
                    HBox.setHgrow(this, Priority.ALWAYS)
                },

                VBox(
                    ImageView(
                        Image("/heast/client/images/settings/client.png")
                    ).apply {
                        this.fitWidth = 50.0
                        this.fitHeight = 50.0
                    },
                    FontManager.boldLabel("This client")
                ).apply {
                    this.alignment = Pos.CENTER
                    this.spacing = 5.0
                },

                Separator(Orientation.HORIZONTAL).apply {
                    this.background = Background.fill(Color.ALICEBLUE)
                    HBox.setHgrow(this, Priority.ALWAYS)
                },

                ImageView(
                    Image("/heast/client/images/settings/disconnected.png")
                ).apply {
                    this.fitWidth = 25.0
                    this.fitHeight = 25.0
                },

                Separator(Orientation.HORIZONTAL).apply {
                    this.background = Background.fill(Color.ALICEBLUE)
                    HBox.setHgrow(this, Priority.ALWAYS)
                },

                VBox(
                    ImageView(
                        Image("/heast/client/images/settings/no-connection.png")
                    ).apply {
                        this.fitWidth = 50.0
                        this.fitHeight = 50.0
                    },
                    FontManager.boldLabel("Not Connected")
                ).apply {
                    this.alignment = Pos.CENTER
                    this.spacing = 5.0
                },
            ).apply {
                this.padding = Insets(10.0, 20.0, 10.0, 20.0)
                this.alignment = Pos.CENTER
                this.spacing = 20.0
                this.isFillHeight = true
            }
        )

        this.backgroundProperty().bind(
            Bindings.createObjectBinding({
                Background(
                    BackgroundFill(
                        Settings.colors["Secondary Color"]!!.color.value,
                        CornerRadii(10.0),
                        Insets.EMPTY
                    )
                )
            }, Settings.colors["Secondary Color"]!!.color)
        )
    }
}