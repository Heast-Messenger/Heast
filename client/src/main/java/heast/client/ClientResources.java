package heast.client;

import java.io.File;
import java.net.URISyntaxException;
import java.util.Objects;

public final class ClientResources {
    public static String getResource(String path) {
        return Objects.requireNonNull(ClientResources.class.getResource(path)).toExternalForm();
    }

    public static File getResourceFile(String path) throws URISyntaxException {
        return new File(Objects.requireNonNull(ClientResources.class.getResource(path)).toURI());
    }
}
