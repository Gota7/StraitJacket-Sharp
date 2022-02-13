namespace ASL.IO;

impl File {

    // Read a raw data stream.
    pri fn readRaw(object* dest, size_t size) {
        
        // Check to make sure we can read.
        if (m_position + size > fileSize) {
            throw Exception("Can't read past end of stream!");
        }

        // Switch file mode.
        switch (fileDataMode => x) {

            // Memory.
            case Memory:
                memcpy(dest, &x.buffer[position], size);
                break;

            // File stream.
            case FileStream:
                fread(dest, size, sizeof(u8), x.fileHandle);
                break;

        }
        m_position += size;

    }

}