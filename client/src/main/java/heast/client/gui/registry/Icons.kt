package heast.client.gui.registry

import heast.client.ClientResources
import javafx.scene.image.Image

object Icons {
	object Logo {
		val BIG : String = ClientResources.getResource("icons/logo/big.png")
		val SMALL : String = ClientResources.getResource("icons/logo/small.png")
	}

	object Menu {
		val BACK : String = ClientResources.getResource("icons/welcome/navigate_back.png")
		val NEXT : String = ClientResources.getResource("icons/welcome/navigate_next.png")
		val SIGNUP : String = ClientResources.getResource("icons/welcome/sign_up.png")
		val LOGIN : String = ClientResources.getResource("icons/welcome/log_in.png")
		val RESET : String = ClientResources.getResource("icons/welcome/reset.png")
	}

	object Verify {
		val EMAIL : String = ClientResources.getResource("icons/verification/email.png")
		val GOOGLE : String = ClientResources.getResource("icons/verification/google.png")
		val VERIFY : String = ClientResources.getResource("icons/verification/verify.png")
		val QR_CODE : String = ClientResources.getResource("icons/verification/TEST_QR_CODE.png")
	}

	fun String.toImg() : Image {
		return Image(this)
	}
}