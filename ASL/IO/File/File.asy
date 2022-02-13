namespace ASL.IO;

pri unsafe enum FileDataMode {
    Memory { byte* buffer },
    FileStream { object* fileHandle }
}

pub unsafe struct File {
    pri FileDataMode fileDataMode;
    pri usize_t m_position;
    pub usize_t fileSize { get; private set; }
}

impl File {

    // All in memory.
    pub File() {
        fileDataMode = Memory { buffer: /* TODO CREATE BUFFER!!! */ };
        m_position = 0;
        fileSize = 0;
        // TODO: INIT MEMORY STREAM!!!
    }

    // Open from a new or existing file. TODO!!!


}