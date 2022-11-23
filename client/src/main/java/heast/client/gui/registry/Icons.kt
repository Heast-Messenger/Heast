package heast.client.gui.registry

import heast.client.ClientResources
import javafx.scene.image.Image

object Icons {
	val logo = mapOf(
		"big" to ClientResources.getResource("icons/logo/big.png"),
		"small" to ClientResources.getResource("icons/logo/small.png"),
	)

	val menu = mapOf(
		"back" to ClientResources.getResource("icons/welcome/navigate_back.png"),
		"next" to ClientResources.getResource("icons/welcome/navigate_next.png"),
		"sign_up" to ClientResources.getResource("icons/welcome/sign_up.png"),
		"log_in" to ClientResources.getResource("icons/welcome/log_in.png"),
	)

	val verify = mapOf(
		"email" to ClientResources.getResource("icons/verification/email.png"),
		"google" to ClientResources.getResource("icons/verification/google.png"),
		"verify" to ClientResources.getResource("icons/verification/verify.png"),
		"TEST_QR_CODE" to ClientResources.getResource("icons/verification/TEST_QR_CODE.png"),
	)

	fun String.toImg() : Image {
		return Image(this)
	}
}